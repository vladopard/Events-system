import { Link } from 'react-router-dom'
import { useState } from 'react'

function EventCard({ event }) {
  const { id, name, description, startDate, venue, city, imageUrl } = event
  const [showFullDescription, setShowFullDescription] = useState(false)

  return (
    <div className="card" style={{ width: '18rem' }}>
      <img src={imageUrl} className="card-img-top" alt={name} />
      <div className="card-body">
        <h5 className="card-title">{name}</h5>

        <p className="card-text">
          {showFullDescription
            ? description
            : description.slice(0, 100) + '...'}
        </p>

        <div className="text-center">
          <button
            className="btn btn-link p-0 mb-2"
            onClick={() => setShowFullDescription(prev => !prev)}
          >
            {showFullDescription ? 'Prikaži manje' : 'Prikaži više'}
          </button>
        </div>

        <p><strong>Datum:</strong> {new Date(startDate).toLocaleString()}</p>
        <p><strong>Lokacija:</strong> {venue}, {city}</p>

        <div className="text-center mt-3">
          <Link to={`/events/${id}`} className="btn btn-primary">Detalji</Link>
        </div>
      </div>
    </div>
  )
}

export default EventCard
