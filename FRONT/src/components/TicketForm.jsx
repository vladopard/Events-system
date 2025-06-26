import React from 'react'

/**
 * mode:
 *  • "ticket"      → Seat + TicketType (стандардни AddTicketPage)
 *  • "ticketType"  → Name + Price      (AddTicketTypePage)
 */
export default function TicketForm({
  mode        = 'ticket',          // <—
  title,
  submitText  = 'Snimi',
  onSubmit,

  /** ticket mode props */
  seat,           setSeat,
  ticketTypeId,   setTicketTypeId,
  ticketTypes     = [],

  /** shared */
  eventId,        setEventId,
  events          = [],

  /** ticketType mode props */
  name,           setName,
  price,          setPrice,

  disabledSubmit  = false,
}) {
  const handleSubmit = e => {
    e.preventDefault()
    onSubmit()
  }

  return (
    <div className="card p-4 shadow-sm" style={{ maxWidth: 500 }}>
      {title && <h5 className="mb-3">{title}</h5>}

      <form onSubmit={handleSubmit}>
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

        {mode === 'ticket' && (
          <>
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
          </>
        )}

        {mode === 'ticketType' && (
          <>
            {/* Naziv tipa */}
            <div className="mb-3">
              <label className="form-label">Naziv tipa</label>
              <input
                type="text"
                className="form-control"
                value={name}
                onChange={e => setName(e.target.value)}
                placeholder="npr. ZLATNA"
                required
              />
            </div>

            {/* Cena */}
            <div className="mb-4">
              <label className="form-label">Cena</label>
              <input
                type="number"
                step="0.01"
                className="form-control"
                value={price}
                onChange={e => setPrice(e.target.value)}
                placeholder="220.00"
                required
              />
            </div>
          </>
        )}

        <button
          className="btn btn-primary w-100"
          disabled={disabledSubmit}
        >
          {submitText}
        </button>
      </form>
    </div>
  )
}
