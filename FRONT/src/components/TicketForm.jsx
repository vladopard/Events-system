import React from 'react'

export default function TicketForm({
  title,
  submitText = 'Snimi',
  onSubmit,
  seat,           setSeat,
  eventId,        setEventId,
  ticketTypeId,   setTicketTypeId,
  events,
  ticketTypes,
  disabledSubmit = false,
}) {
  return (
    <div className="card p-4 shadow-sm" style={{ maxWidth: 500 }}>
      <h5 className="mb-3">{title}</h5>

      {/* Događaj */}
      <div className="mb-3">
        <label className="form-label">Događaj</label>
        <select
          className="form-select"
          value={eventId}
          onChange={e => setEventId(e.target.value)}
          required
        >
          <option value="">-- izaberi događaj --</option>
          {events.map(ev => (
            <option key={ev.id} value={ev.id}>{ev.name}</option>
          ))}
        </select>
      </div>

      {/* Tip karte */}
      <div className="mb-3">
        <label className="form-label">Tip karte</label>
        <select
          className="form-select"
          value={ticketTypeId}
          onChange={e => setTicketTypeId(e.target.value)}
          required
          disabled={!eventId}
        >
          <option value="">-- izaberi tip --</option>
          {ticketTypes.map(tt => (
            <option key={tt.id} value={tt.id}>
              {tt.name}{tt.price ? ` — $${tt.price}` : ''}
            </option>
          ))}
        </select>
      </div>

      {/* Seat */}
      <div className="mb-4">
        <label className="form-label">Seat</label>
        <input
          type="text"
          className="form-control"
          value={seat}
          onChange={e => setSeat(e.target.value)}
          placeholder="npr. GD1"
          required
        />
      </div>

      <button
        className="btn btn-primary w-100"
        disabled={disabledSubmit}
        onClick={onSubmit}
      >
        {submitText}
      </button>
    </div>
  )
}
