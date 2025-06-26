import { useEffect, useState } from 'react';
import { api } from '../../services/api';
import TicketForm from '../../components/TicketForm';

function AddTicketPage() {
  const [events, setEvents] = useState([]);
  const [ticketTypes, setTicketTypes] = useState([]);
  const [selectedEvent, setSelectedEvent] = useState('');
  const [form, setForm] = useState({ seat: '', ticketTypeId: '' });
  const [msg, setMsg] = useState(null);

  /* učitaj sve događaje */
  useEffect(() => {
    api
      .get('/events', { params: { pageSize: 50 } })
      .then((res) => setEvents(res.data))
      .catch(() =>
        setMsg({ type: 'danger', text: 'Greška pri učitavanju događaja.' })
      );
  }, []);

  /* učitaj tipove karata za izabrani događaj */
  useEffect(() => {
    if (!selectedEvent) return;
    api
      .get(`/tickettypes/by-event/${selectedEvent}`)
      .then((res) => setTicketTypes(res.data))
      .catch(() =>
        setMsg({ type: 'danger', text: 'Greška pri učitavanju tipova karata.' })
      );
  }, [selectedEvent]);

  const handleSubmit = async () => {
    setMsg(null);
    try {
      await api.post('/tickets', {
        seat: form.seat,
        eventId: Number(selectedEvent),
        ticketTypeId: Number(form.ticketTypeId),
      });
      setMsg({ type: 'success', text: 'Karta uspešno dodata!' });
      setForm({ seat: '', ticketTypeId: '' });
    } catch {
      setMsg({ type: 'danger', text: 'Neuspešno dodavanje karte.' });
    }
  };

  return (
    <>
      {msg && <div className={`alert alert-${msg.type}`}>{msg.text}</div>}

      <TicketForm
        title="Dodaj novu kartu"
        submitText="Sačuvaj kartu"
        onSubmit={handleSubmit}
        seat={form.seat}
        setSeat={(v) => setForm({ ...form, seat: v })}
        eventId={selectedEvent}
        setEventId={(v) => {
          setSelectedEvent(v);
          setForm({ ...form, ticketTypeId: '' });
        }}
        ticketTypeId={form.ticketTypeId}
        setTicketTypeId={(v) => setForm({ ...form, ticketTypeId: v })}
        events={events}
        ticketTypes={ticketTypes}
      />
    </>
  );
}

export default AddTicketPage;
