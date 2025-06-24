import { useEffect, useState } from 'react'
import { api } from '../../services/api'

function AddTicketPage() {
    const [events, setEvents] = useState([])
    const [ticketTypes, setTicketTypes] = useState([])
    const [selectedEvent, setSelectedEvent] = useState('')
    const [form, setForm] = useState({ seat: '', ticketTypeId: '' })
    const [msg, setMsg] = useState(null)

    /* -- učitaj sve događaje za dropdown -- */
    useEffect(() => {
        api.get('/events')
            .then(res => setEvents(res.data))
            .catch(() => setMsg({ type: 'danger', text: 'Greška pri učitavanju događaja.' }))
    }, [])

    /* -- kad se izabere event, učitamo tipove karata vezane za njega -- */
    useEffect(() => {
        if (!selectedEvent) return
        api.get(`/events/${selectedEvent}/ticketTypes`)
            .then(res => setTicketTypes(res.data))
            .catch(() => setMsg({ type: 'danger', text: 'Greška pri učitavanju tipova karata.' }))
    }, [selectedEvent])

    const handleSubmit = async (e) => {
        e.preventDefault()
        setMsg(null)

        try {
            await api.post('/tickets', {
                seat: form.seat,
                eventId: Number(selectedEvent),
                ticketTypeId: Number(form.ticketTypeId),
            })

            setMsg({ type: 'success', text: 'Karta uspešno dodata!' })
            setForm({ seat: '', ticketTypeId: '' })
        } catch {
            setMsg({ type: 'danger', text: 'Neuspešno dodavanje karte.' })
        }
    }

    return (
        <>
            {msg && <div className={`alert alert-${msg.type}`}>{msg.text}</div>}

            <form onSubmit={handleSubmit} className="card p-4" style={{ maxWidth: 500 }}>
                {/* Event */}
                <div className="mb-3">
                    <label className="form-label">Događaj</label>
                    <select
                        className="form-select"
                        value={selectedEvent}
                        onChange={(e) => {
                            setSelectedEvent(e.target.value)
                            setForm({ ...form, ticketTypeId: '' }) // reset tipa
                        }}
                        required
                    >
                        <option value="">-- izaberi događaj --</option>
                        {events.map(ev => (
                            <option key={ev.id} value={ev.id}>{ev.name}</option>
                        ))}
                    </select>
                </div>

                {/* Ticket type */}
                <div className="mb-3">
                    <label className="form-label">Tip karte</label>
                    <select
                        className="form-select"
                        value={form.ticketTypeId}
                        onChange={e => setForm({ ...form, ticketTypeId: e.target.value })}
                        required
                        disabled={!selectedEvent}
                    >
                        <option value="">-- izaberi tip --</option>
                        {ticketTypes.map(tt => (
                            <option key={tt.id} value={tt.id}>{tt.name} — ${tt.price}</option>
                        ))}
                    </select>
                </div>

                {/* Seat */}
                <div className="mb-3">
                    <label className="form-label">Seat</label>
                    <input
                        type="text"
                        className="form-control"
                        value={form.seat}
                        onChange={e => setForm({ ...form, seat: e.target.value })}
                        placeholder="npr. GD1"
                        required
                    />
                </div>

                <button type="submit" className="btn btn-primary w-100">
                    Sačuvaj kartu
                </button>
            </form>
        </>
    )
}

export default AddTicketPage
