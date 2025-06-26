import { useEffect, useState } from 'react'
import { api } from '../../services/api'
import TicketForm from '../../components/TicketForm'

export default function AddTicketTypePage() {
  const [events, setEvents] = useState([])
  const [form, setForm]     = useState({ eventId: '', name: '', price: '' })
  const [msg,  setMsg]      = useState(null)
  const [saving, setSaving] = useState(false)

  /* događaji za dropdown */
  useEffect(() => {
    api.get('/events', { params: { pageSize: 50 } })
       .then(r => setEvents(r.data))
       .catch(() =>
         setMsg({ type: 'danger', text: 'Greška pri učitavanju događaja.' })
       )
  }, [])

  /* submit */
  const handleSubmit = async () => {
    setSaving(true)
    setMsg(null)
    try {
      await api.post('/tickettypes', {
        eventId: Number(form.eventId),
        name:    form.name.trim(),
        price:   Number(form.price)
      })
      setMsg({ type: 'success', text: 'Tip karte uspešno dodat!' })
      setForm({ eventId: form.eventId, name: '', price: '' }) // задржи изабрани event
    } catch {
      setMsg({ type: 'danger', text: 'Neuspešno dodavanje tipa.' })
    } finally {
      setSaving(false)
    }
  }

  return (
    <>
      {msg && (
        <div className={`alert alert-${msg.type} mb-3`}>{msg.text}</div>
      )}

      <TicketForm
        mode="ticketType"
        title="Dodaj tip karte"
        submitText="Sačuvaj tip"
        onSubmit={handleSubmit}

        /* shared */
        eventId={form.eventId}
        setEventId={v => setForm({ ...form, eventId: v })}

        events={events}

        /* ticketType‑mode */
        name={form.name}
        setName={v => setForm({ ...form, name: v })}
        price={form.price}
        setPrice={v => setForm({ ...form, price: v })}

        disabledSubmit={saving}
      />
    </>
  )
}
