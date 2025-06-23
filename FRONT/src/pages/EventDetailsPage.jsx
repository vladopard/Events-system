import { useParams } from 'react-router-dom'
import { useEffect, useState } from 'react'
import { api } from '../services/api'

function EventDetailsPage() {
    const { id } = useParams()
    const [event, setEvent] = useState(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState(null)
    const [showFullDescription, setShowFullDescription] = useState(false)

    useEffect(() => {
        api.get(`/events/${id}`)
            .then(res => {
                setEvent(res.data)
                setLoading(false)
            })
            .catch(err => {
                setError('Greška pri učitavanju događaja.')
                setLoading(false)
            })
    }, [id])

    if (loading) return <div className="text-center mt-4">Učitavanje...</div>
    if (error) return <div className="alert alert-danger">{error}</div>
    if (!event) return null

    return (
        <div className="container mt-4">
            <h1 className="mb-4">{event.name}</h1>

            <div className="row">
                <div className="col-md-6">
                    <img
                        src={event.imageUrl}
                        alt={event.name}
                        className="img-fluid rounded"
                    />
                </div>

                <div className="col-md-6">
                    <p>
                        <strong>Opis:</strong>{' '}
                        {showFullDescription
                            ? event.description
                            : event.description.slice(0, 400) + '...'}
                    </p>
                    <div>
                        <button
                            className="btn btn-link p-0 mb-3"
                            onClick={() => setShowFullDescription(prev => !prev)}
                        >
                            {showFullDescription ? 'Prikaži manje' : 'Prikaži više'}
                        </button>
                    </div>

                    <p><strong>Datum:</strong> {new Date(event.startDate).toLocaleString()}</p>
                    <p><strong>Lokacija:</strong> {event.venue}, {event.city}</p>
                </div>
            </div>

            <hr className="my-4" />

            <h2 className="mb-3">Dostupne karte</h2>
            <div className="row">
                {event.ticketTypes.map(ticket => (
                    <div key={ticket.id} className="col-md-4 mb-3">
                        <div className="card h-100">
                            <div className="card-body">
                                <h5 className="card-title">{ticket.name}</h5>
                                <p className="card-text">${ticket.price.toFixed(2)}</p>
                                <button className="btn btn-success">Kupi</button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default EventDetailsPage
