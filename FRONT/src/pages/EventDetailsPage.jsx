import { useParams } from 'react-router-dom'
import { useEffect, useState } from 'react'
import { api } from '../services/api'
import { useAuth } from '../context/AuthContext'

function EventDetailsPage() {
    const { id } = useParams()

    const [event, setEvent] = useState(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState(null)
    const [showFullDescription, setShow] = useState(false)
    const [feedback, setFeedback] = useState(null)
    const { user } = useAuth()

    /* ------------------ FETCH EVENT ------------------ */
    useEffect(() => {
        api.get(`/events/${id}`)
            .then(res => { setEvent(res.data); setLoading(false) })
            .catch(() => { setError('Greška pri učitavanju događaja.'); setLoading(false) })
    }, [id])

    /* ------------------ BUY / QUEUE ----------------*/
    const handleBuyTicket = async (ticketTypeId) => {
        if (!user) {
            setFeedback({ type: 'error', msg: 'Morate biti prijavljeni da biste kupili kartu.' })
            return
        }

        try {
            const res = await api.post('/order/order-or-queue', {
                userId: user.userId,
                ticketTypeId: ticketTypeId,
            })

            const data = res.data

            if (data.isQueued) {
                setFeedback({
                    type: 'queue',
                    msg: `Nema karata. Stavljeni ste u red za: ${data.queue.ticketTypeName} (ID reda: ${data.queue.id})`,
                })
            } else {
                setFeedback({
                    type: 'order',
                    msg: `Uspešno ste kupili kartu! Order ID: ${data.order.id}`,
                })
            }

        } catch (err) {
            setFeedback({ type: 'error', msg: 'Greška pri kupovini karte.' })
            console.error(err)
        }
    }


    /* ------------------ UI ------------------ */
    if (loading) return <div className="text-center mt-4">Učitavanje...</div>
    if (error) return <div className="alert alert-danger">{error}</div>
    if (!event) return null

    return (
        <div className="container mt-4">
            {/* FEEDBACK ALERT ------------------------------------------------ */}
            {feedback && (
                <div className={`alert ${feedback.type === 'order'
                    ? 'alert-success'
                    : feedback.type === 'queue'
                        ? 'alert-warning'
                        : 'alert-danger'} mb-4`}>
                    {feedback.msg}
                </div>
            )}

            {/* HEADER -------------------------------------------------------- */}
            <h1 className="mb-4">{event.name}</h1>

            <div className="row">
                <div className="col-md-6">
                    <img src={event.imageUrl} alt={event.name} className="img-fluid rounded" />
                </div>

                <div className="col-md-6">
                    <p>
                        <strong>Opis:</strong>{' '}
                        {showFullDescription
                            ? event.description
                            : event.description.slice(0, 300) + '...'}
                    </p>
                    <button
                        className="btn btn-link p-0 mb-3"
                        onClick={() => setShow(prev => !prev)}
                    >
                        {showFullDescription ? 'Prikaži manje' : 'Prikaži više'}
                    </button>

                    <p><strong>Datum:</strong> {new Date(event.startDate).toLocaleString()}</p>
                    <p><strong>Lokacija:</strong> {event.venue}, {event.city}</p>
                </div>
            </div>

            {/* TICKETS ------------------------------------------------------- */}
            <hr className="my-4" />
            <h2 className="mb-3">Dostupne karte</h2>
            <div className="row">
                {event.ticketTypes.map(t => (
                    <div key={t.id} className="col-md-4 mb-3">
                        <div className="card h-100">
                            <div className="card-body d-flex flex-column">
                                <h5 className="card-title">{t.name}</h5>
                                <p className="card-text">${t.price.toFixed(2)}</p>
                                <button
                                    className="btn btn-success mt-auto"
                                    onClick={() => handleBuyTicket(t.id)}
                                >
                                    Kupi
                                </button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default EventDetailsPage
