import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { api } from '../services/api';

function ProfilePage() {
  const { user } = useAuth();

  const [orders, setOrders] = useState([]);
  const [queues, setQueues] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!user) return;

    const load = async () => {
      try {
        const [ordersRes, queuesRes] = await Promise.all([
          api.get(`/order/by-user/${user.userId}`),
          api.get(`/queues/user/${user.userId}`)
        ]);
        setOrders(ordersRes.data);
        setQueues(queuesRes.data);
      } catch {
        setError('Greška pri učitavanju podataka.');
      }
    };

    load();
  }, [user?.userId]);

  const statusText = s =>
    s === 0 ? 'Čeka se' : s === 1 ? 'Rezervisano' : 'Završen';

  const statusClass = s =>
    s === 0 ? 'text-warning' : s === 1 ? 'text-info' : 'text-success';

  const handleCancelQueue = async (queueId) => {
    try {
      await api.delete(`/queues/${queueId}`);
      setQueues(prev => prev.filter(q => q.id !== queueId));
    } catch {
      alert('Greška pri otkazivanju čekanja.');
    }
  };

  if (!user) {
    return (
      <div className="container mt-4">
        Morate biti prijavljeni da biste videli profil.
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Moj profil</h2>

      {/* osnovni podaci */}
      <div className="card mb-4">
        <div className="card-body">
          <p><strong>Ime:</strong>   {user.firstName}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p><strong>Uloga:</strong> {Array.isArray(user.roles) ? user.roles.join(', ') : user.roles}</p>
        </div>
      </div>

      {error && <div className="alert alert-danger">{error}</div>}

      {/* kupljene karte */}
      <h4 className="mb-3">🎟 Kupljene karte</h4>
      {orders.length === 0 ? (
        <p>Nemate nijednu kupljenu kartu.</p>
      ) : (
        <div className="row">
          {orders.map(o => (
            <div key={o.id} className="col-md-6 col-lg-4 mb-3">
              <div className="card h-100">
                <div className="card-body">
                  <p><strong>Order ID:</strong> {o.id}</p>
                  <p><strong>Datum:</strong> {new Date(o.createdAt).toLocaleString()}</p>

                  {o.tickets.map(t => (
                    <div key={t.id} className="border-top pt-2 mt-2">
                      <p className="mb-1"><strong>Event:</strong> {t.eventName}</p>
                      <p className="mb-1"><strong>Sedište:</strong> {t.seat}</p>
                      <p className="mb-1"><strong>Tip karte:</strong> {t.ticketTypeName}</p>
                      <p className="mb-0"><strong>Cena:</strong> {t.ticketPrice.toLocaleString('sr-RS', { style: 'currency', currency: 'RSD' })}</p>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <hr className="my-4" />

      {/* red čekanja */}
      <h4 className="mb-3">⏳ Red čekanja</h4>
      {queues.length === 0 ? (
        <p>Niste u nijednom redu za kartu.</p>
      ) : (
        <div className="row">
          {queues.map(q => (
            <div key={q.id} className="col-md-6 col-lg-4 mb-3">
              <div className="card h-100">
                <div className="card-body d-flex flex-column">
                  <div className="d-flex justify-content-between">
                    <p className="mb-2"><strong>ID reda:</strong> {q.id}</p>
                    <span className={statusClass(q.status)}>{statusText(q.status)}</span>
                  </div>

                  <p className="mb-1"><strong>Event:</strong> {q.eventName}</p>
                  <p className="mb-1"><strong>Tip karte:</strong> {q.ticketTypeName}</p>
                  <p className="mb-0"><strong>Cena:</strong> {q.price.toLocaleString('sr-RS', { style: 'currency', currency: 'RSD' })}</p>

                  {q.status === 0 && (
                    <button
                      className="btn btn-sm btn-outline-danger mt-3 align-self-start"
                      onClick={() => handleCancelQueue(q.id)}
                    >
                      ❌ Otkaži čekanje
                    </button>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default ProfilePage;
