import { useEffect, useState } from 'react'
import { api } from '../services/api'
import EventCard from '../components/EventCard'

export default function EventsPage() {
  const pageSize = 6

  const [page, setPage] = useState(1)
  const [search, setSearch] = useState('')      // ← термин за претрагу
  const [events, setEvents] = useState([])
  const [meta, setMeta] = useState(null)        // X‑Pagination
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  /* ---- fetch кад год се page или search промени ---- */
  useEffect(() => {
    setLoading(true)

    api.get('/events', {
      params: {
        pageNumber: page,
        pageSize,
        search: search || undefined   // празан string -> undefined -> не шаље се
      }
    })
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
  }, [page, search])

  /* ---- submit претраге ресетује на прву страницу ---- */
  const handleSearchSubmit = e => {
    e.preventDefault()
    const term = e.target.elements.search.value.trim()
    setPage(1)          // старт од прве стране
    setSearch(term)
  }

  if (loading) return <p>Loading…</p>
  if (error) return <p>{error}</p>

  const prevDisabled = meta && !meta.hasPrevious
  const nextDisabled = meta && !meta.hasNext

  return (
    <div className="container mt-4">
      <h1 className="mb-4">Events</h1>

      {/* search bar */}
      <form
        onSubmit={handleSearchSubmit}
        className="d-flex align-items-center gap-2 mb-4"
      >
        <input
          name="search"
          defaultValue={search}
          className="form-control"
          placeholder="Search events…"
        />
        <button className="btn btn-primary">Search</button>
      </form>

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
            className={`btn ${prevDisabled ? 'btn-secondary disabled' : 'btn-outline-primary'}`}
            disabled={prevDisabled}
            onClick={() => setPage(p => p - 1)}
          >
            ← Prev
          </button>

          <span>{meta.currentPage} / {meta.totalPages}</span>

          <button
            className={`btn ${nextDisabled ? 'btn-secondary disabled' : 'btn-outline-primary'}`}
            disabled={nextDisabled}
            onClick={() => setPage(p => p + 1)}
          >
            Next →
          </button>
        </div>
      )}
    </div>
  )
}
