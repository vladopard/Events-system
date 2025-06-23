import { useEffect, useState } from "react";
import { api } from '../services/api'
import EventCard from "../components/EventCard";

export default function EventsPage() {
    const [events, setEvents] = useState([])
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        api.get('/events')
            .then(res => {
                setEvents(res.data)
                setLoading(false)
            })
            .catch(err => {
                setError("Error while loading data")
                setLoading(false)
            })
    }, [])

    if (loading) return <p>Loading...</p>
    if (error) return <p>{error}</p>

    return (
        <div className="container mt-4">
            <h1 className="mb-4">Events</h1>
            <div className="row g-4">
                {events.map(event => (
                    <div key={event.id} className="col-md-4">
                        <EventCard event={event} />
                    </div>
                ))}
            </div>
        </div>

    )

}