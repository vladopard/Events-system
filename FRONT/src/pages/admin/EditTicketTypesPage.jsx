import { useEffect, useState } from 'react'
import { api } from '../../services/api'
import TicketForm from '../../components/TicketForm'

export default function EditTicketTypesPage() {
  const [events, setEvents] = useState([])
  const [ticketTypes, setTicketTypes] = useState([])

  const [eventId, setEventId] = useState('')
  const [editing, setEditing] = useState(null)

  const [form, setForm] = useState({ name: '', price: '', eventId: '' })
  const [saving, setSaving] = useState(false)
  const [msg, setMsg] = useState('')

  /* učitaj sve događaje */
  useEffect(() => {
    api.get('/events', { params: { pageSize: 50 }})
      .then(res => setEvents(res.data))
      .catch(() => setMsg('Greška pri učitavanju događaja.'))
  }, [])

  /* učitaj sve tipove za izabrani događaj */
  useEffect(() => {
    if (!eventId) {
      setTicketTypes([])
      return
    }
    api.get(`/tickettypes/by-event/${eventId}`)
      .then(res => setTicketTypes(res.data))
      .catch(() => setMsg('Greška pri učitavanju tipova karata.'))
  }, [eventId])

  /* submit izmene */
  const handleUpdate = async () => {
    if (!editing) return

    setSaving(true)
    setMsg('')
    try {
      await api.put(`/tickettypes/${editing.id}`, {
        name: form.name.trim(),
        price: Number(form.price),
        eventId: Number(form.eventId)
      })
      setEditing(null)
      setForm({ name: '', price: '', eventId }) // resetuj, zadrži event
      const refreshed = await api.get(`/tickettypes/by-event/${eventId}`)
      setTicketTypes(refreshed.data)
    } catch {
      setMsg('Neuspešna izmena tipa karte.')
    } finally {
      setSaving(false)
    }
  }

  return (
    <>
      {/* FILTER */}
      <div className="card p-4 shadow-sm mb-4" style={{ maxWidth: 500 }}>
        <h5 className="mb-3">Izaberi događaj</h5>

        <select
          className="form-select mb-2"
          value={eventId}
          onChange={e => {
            setEventId(e.target.value)
            setEditing(null)
            setMsg('')
          }}
        >
          <option value="">— Izaberi događaj —</option>
          {events.map(ev => (
            <option key={ev.id} value={ev.id}>{ev.name}</option>
          ))}
        </select>

        {msg && <div className="alert alert-info mb-0">{msg}</div>}
      </div>

      {/* LISTA TIPA */}
      {ticketTypes.length > 0 && (
        <div className="row">
          {ticketTypes.map(tt => (
            <div key={tt.id} className="col-md-6 col-lg-4 mb-3">
              <div className="card h-100">
                <div className="card-body d-flex justify-content-between align-items-start">
                  <div>
                    <h6 className="mb-1">{tt.name}</h6>
                    <p className="mb-0 text-muted">{tt.price.toFixed(2)} RSD</p>
                  </div>
                  <button
                    className="btn btn-sm btn-outline-secondary"
                    onClick={() => {
                      setEditing(tt)
                      setForm({
                        name: tt.name,
                        price: tt.price,
                        eventId: eventId
                      })
                    }}
                  >
                    ✏️
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* FORMA ZA IZMENE */}
      {editing && (
        <div className="card p-4 shadow-sm" style={{ maxWidth: 500 }}>
          <TicketForm
            title={`Izmena tipa #${editing.id}`}
            submitText={saving ? 'Čuvam…' : 'Snimi izmene'}
            onSubmit={handleUpdate}
            mode="ticketType"
            eventId={form.eventId}
            setEventId={v => setForm({ ...form, eventId: v })}
            name={form.name}
            setName={v => setForm({ ...form, name: v })}
            price={form.price}
            setPrice={v => setForm({ ...form, price: v })}
            events={events}
            disabledSubmit={saving}
          />

          <button
            className="btn btn-link mt-3 p-0"
            onClick={() => setEditing(null)}
          >
            ↩️ Odustani od izmene
          </button>
        </div>
      )}
    </>
  )
}
