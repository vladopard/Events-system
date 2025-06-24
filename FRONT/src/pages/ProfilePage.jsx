import { useEffect, useState } from 'react'
import { useAuth } from '../context/AuthContext'
import { api } from '../services/api'
import { Navigate } from 'react-router-dom' 

function ProfilePage() {
  const { user } = useAuth()

  /* --------------- HOOK-ови морају ићи први ---------------- */
  const [orders, setOrders] = useState([])
  const [queues, setQueues] = useState([])
  const [error,  setError ] = useState(null)

  /* ------------ Fetch kupljene karte & queue -------------- */
  useEffect(() => {
    if (!user) return        // safety guard

    const fetchData = async () => {
      try {
        const [ordersRes, queuesRes] = await Promise.all([
          api.get(`/user/${user.userId}/orders`),
          api.get(`/user/${user.userId}/queues`)
        ])
        setOrders(ordersRes.data)
        setQueues(queuesRes.data)
      } catch {
        setError('Greška pri učitavanju podataka.')
      }
    }
    fetchData()
  }, [user?.userId])          // depend samo na userId

  /* ---------- Ако није логован: прикажи ---------- */
  if (!user) {
    return (
      <div className="container mt-4">
        Morate biti prijavljeni da biste videli profil.
      </div>
    )
  }

  /* ------------------------ UI ----------------------------- */
  return (
    <div className="container mt-4">
      <h2 className="mb-4">Moj profil</h2>

      {/* User info */}
      <div className="card mb-4">
        <div className="card-body">
          <p><strong>Ime:</strong> {user.firstName}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p>
            <strong>Uloga:</strong>{' '}
            {Array.isArray(user.roles) ? user.roles.join(', ') : user.roles}
          </p>
        </div>
      </div>

      {error && <div className="alert alert-danger">{error}</div>}

      {/* Kupljene karte */}
      <h4 className="mb-3">🎟 Kupljene karte</h4>
      {orders.length === 0 ? (
        <p>Nemate nijednu kupljenu kartu.</p>
      ) : (
        <div className="row">
          {orders.map(o => (
            <div key={o.id} className="col-md-4 mb-3">
              <div className="card h-100">
                <div className="card-body">
                  <p><strong>Order ID:</strong> {o.id}</p>
                  <p><strong>Datum:</strong> {new Date(o.createdAt).toLocaleString()}</p>
                  <p><strong>Karte:</strong> {o.ticketIds.join(', ')}</p>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <hr className="my-4" />

      {/* Red čekanja */}
      <h4 className="mb-3">⏳ Red čekanja</h4>
      {queues.length === 0 ? (
        <p>Niste u nijednom redu za kartu.</p>
      ) : (
        <div className="row">
          {queues.map(q => (
            <div key={q.id} className="col-md-4 mb-3">
              <div className="card h-100">
                <div className="card-body">
                  <p><strong>ID reda:</strong> {q.id}</p>
                  <p><strong>Karta:</strong> {q.ticketTypeName}</p>
                  <p><strong>Status:</strong> {q.status === 0 ? 'Čeka se' : 'Završeno'}</p>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default ProfilePage
