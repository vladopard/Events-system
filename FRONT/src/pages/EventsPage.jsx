import { useEffect, useState } from 'react'
import { api } from '../services/api'
import EventCard from '../components/EventCard'

export default function EventsPage() {
  const pageSize = 6                 // колико по страни

  const [page, setPage] = useState(1)
  const [events, setEvents] = useState([])
  const [meta, setMeta] = useState(null)   // X-Pagination
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  /* ---- сваки пут кад се page промени, повуци нову страну ---- */
  useEffect(() => {
    setLoading(true)

    api.get('/events', { params: { pageNumber: page, pageSize } })
      .then(res => {
        setEvents(res.data)

        const head = res.headers['x-pagination']
        setMeta(head ? JSON.parse(head) : null)

        setLoading(false)
      })
      .catch(() => {
        setError('Greška pri učitavanju podataka.')
        setLoading(false)
      })
  }, [page])

  if (loading) return <p>Loading…</p>
  if (error) return <p>{error}</p>

  const prevDisabled = !meta.hasPrevious;
  const nextDisabled = !meta.hasNext;

  return (
    <div className="container mt-4">
      <h1 className="mb-4">Events</h1>

      {/* cards */}
      <div className="row g-4">
        {events.map(ev => (
          <div key={ev.id} className="col-md-4">
            <EventCard event={ev} />
          </div>
        ))}
      </div>

      {/* paginator */}
      {meta && (
        <div className="d-flex justify-content-center gap-3 mt-4">
          <button
            className={`btn ${!meta.hasPrevious ? 'btn-secondary disabled' : 'btn-outline-primary'}`}
            disabled={!meta.hasPrevious}
            onClick={() => setPage(p => p - 1)}
          >
            ← Prev
          </button>

          <span>{meta.currentPage} / {meta.totalPages}</span>

          <button
            className={`btn ${!meta.hasNext ? 'btn-secondary disabled' : 'btn-outline-primary'}`}
            disabled={!meta.hasNext}
            onClick={() => setPage(p => p + 1)}
          >
            Next →
          </button>
        </div>
      )}
    </div>
  );

}
