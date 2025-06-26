import { useEffect, useState } from 'react';
import { api } from '../../services/api';
import TicketForm from '../../components/TicketForm';

export default function EditTicketsPage() {
    const [events, setEvents] = useState([]);
    const [ticketTypes, setTicketTypes] = useState([]);
    const [tickets, setTickets] = useState([]);
    const [eventId, setEventId] = useState('');
    const [ticketTypeId, setTicketTypeId] = useState('');
    const [msg, setMsg] = useState('');
    const [editing, setEditing] = useState(null);
    const [seat, setSeat] = useState('');
    const [saving, setSaving] = useState(false);

    /* svi eventi */
    useEffect(() => {
        api
            .get('/events')
            .then((res) => setEvents(res.data))
            .catch(() => setMsg('Greška pri učitavanju događaja.'));
    }, []);

    /* tipovi karata za izabrani event */
    useEffect(() => {
        if (!eventId) {
            setTicketTypes([]);
            return;
        }
        api
            .get(`/tickettypes/by-event/${eventId}`)
            .then((res) => setTicketTypes(res.data))
            .catch(() => setMsg('Greška pri učitavanju tipova karata.'));
    }, [eventId]);

    /* filtriraj karte */
    const loadTickets = () => {
        if (!eventId || !ticketTypeId) {
            setMsg('Izaberi događaj i tip karte.');
            return;
        }
        setMsg('');
        api
            .get('/tickets/by-event-and-type', {
                params: { eventId, ticketTypeId },
            })
            .then((res) => setTickets(res.data))
            .catch(() => setMsg('Greška pri učitavanju karata.'));
    };

    /* PUT update */
    const handleUpdate = async () => {
        if (!editing) return;
        setSaving(true);
        try {
            await api.put(`/tickets/${editing.id}`, {
                seat,
                eventId,
                ticketTypeId,
            });
            setEditing(null);
            loadTickets();
        } finally {
            setSaving(false);
        }
    };

    return (
        <>
            {/* FILTER-KARTICA */}
            <div className="card p-4 shadow-sm mb-4" style={{ maxWidth: 500 }}>
  <h5 className="mb-3">Filtriraj karte</h5>

  <div className="mb-3">
    <label className="form-label">Događaj</label>
    <select
      className="form-select"
      value={eventId}
      onChange={(e) => {
        setEventId(e.target.value);
        setTicketTypeId('');
        setTickets([]);
      }}
    >
      <option value="">— Izaberi događaj —</option>
      {events.map((ev) => (
        <option key={ev.id} value={ev.id}>
          {ev.name}
        </option>
      ))}
    </select>
  </div>

  <div className="mb-3">
    <label className="form-label">Tip karte</label>
    <select
      className="form-select"
      value={ticketTypeId}
      onChange={(e) => setTicketTypeId(e.target.value)}
      disabled={!ticketTypes.length}
    >
      <option value="">— Izaberi tip karte —</option>
      {ticketTypes.map((tt) => (
        <option key={tt.id} value={tt.id}>
          {tt.name}
        </option>
      ))}
    </select>
  </div>

  <button className="btn btn-primary w-100 mb-3" onClick={loadTickets}>
    Prikaži
  </button>

                {tickets.length === 0 && !msg && (
                    <p className="text-muted">No tickets of this type exist.</p>
                )}

                {tickets.length > 0 && (
                    <table className="table table-sm">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Sedište</th>
                                <th>Status</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {tickets.map((t) => (
                                <tr key={t.id}>
                                    <td>{t.id}</td>
                                    <td>{t.seat}</td>
                                    <td>{t.orderId ? 'Prodata' : 'Slobodna'}</td>
                                    <td>
                                        <button
                                            className="btn btn-sm btn-outline-secondary"
                                            onClick={() => {
                                                setEditing(t);
                                                setSeat(t.seat);
                                                setEventId(t.eventId);
                                                setTicketTypeId(t.ticketTypeId);
                                            }}
                                        >
                                            ✏️
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </div>

            {/* MODAL ZA IZMENU KARTE */}
            {editing && (
                <div className="modal-backdrop d-flex align-items-center justify-content-center">
                    <div className="bg-white rounded p-4 shadow-lg position-relative">
                        <button
                            className="btn-close position-absolute top-0 end-0 m-3"
                            onClick={() => setEditing(null)}
                        />
                        <TicketForm
                            title={`Izmena karte #${editing.id}`}
                            submitText={saving ? 'Čuvam…' : 'Snimi izmene'}
                            onSubmit={handleUpdate}
                            seat={seat}
                            setSeat={setSeat}
                            eventId={eventId}
                            setEventId={setEventId}
                            ticketTypeId={ticketTypeId}
                            setTicketTypeId={setTicketTypeId}
                            events={events}
                            ticketTypes={ticketTypes}
                            disabledSubmit={saving}
                        />
                    </div>
                </div>
            )}
        </>
    );

}
