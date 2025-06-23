import { useEffect, useState } from 'react'
import { api } from '../services/api'
import EventCard from '../components/EventCard'

function HomePage() {
    const [events, setEvents] = useState([])

    useEffect(() => {
        api.get('/events')
            .then(res => setEvents(res.data.slice(0, 4)))
            .catch(err => console.error(err))
    }, [])

    return (
        <div>
            {/* Welcome text */}
            <div className="container mt-4 text-center">
                <h1>Dobrodošli u EventSystem</h1>
                <p className="lead">Kupite karte za najpopularnije događaje</p>
            </div>

            {/* Hero image - full width, smaller height */}
            <div style={{ width: '100vw' }}>
                <img
                    src='https://i.pinimg.com/originals/33/17/cd/3317cd1a2308615b5ae65917739a365f.jpg'
                    alt="Promo"
                    style={{
                        width: '100vw',
                        height: '300px',
                        objectFit: 'cover',
                        display: 'block',
                    }}
                />
            </div>


            {/* Grid of events, centered with spacing on sides */}
            <div className="container my-5 d-flex justify-content-center">
                <div className="row g-4" style={{ maxWidth: '960px', width: '100%' }}>
                    {events.map((event, index) => (
                        <div key={event.id} 
                        className={`col-md-6 d-flex justify-content-center slide-in-${index % 2 === 0 ? 'left' : 'right'}`}>
                            <EventCard event={event} />
                        </div>
                    ))}
                </div>
            </div>


        </div>
    )
}

export default HomePage
