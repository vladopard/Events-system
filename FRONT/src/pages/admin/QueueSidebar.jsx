import { useEffect, useState } from 'react'
import { api } from '../../services/api'

export default function QueueSidebar() {
    const [queues, setQueues] = useState([])
    const [error, setError] = useState(null)

    useEffect(() => {
        api.get('/queues/waiting') 
            .then(res => setQueues(res.data))
            .catch(() => setError('Greška pri učitavanju reda čekanja.'))
    }, [])

    return (
        <div
            className="bg-light border rounded p-3 shadow-sm"
            style={{ width: '300px', minHeight: '100%' }}
        >
            <h6 className="mb-3">⏳ Red čekanja</h6>

            {error && <div className="text-danger">{error}</div>}

            {queues.length === 0 && !error && (
                <p className="text-muted">Nema čekanja.</p>
            )}

            <ul className="list-unstyled mb-0">
                {queues.map(q => (
                    <li key={q.id} className="mb-3 border-bottom pb-2">
                        👤 <strong>{q.userId}</strong><br />
                        🎟 Tip: {q.ticketTypeName}<br />
                        📅 Događaj: {q.eventName || 'n/a'}
                    </li>
                ))}
            </ul>
        </div>
    )

}
