import { useState } from 'react'
import AddTicketPage from './AddTicketPage'
import EditTicketsPage from './EditTicketsPage'

function AdminDashboard() {
  const [panel, setPanel] = useState('')   // '', 'add', 'edit'

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Admin Dashboard</h2>

      <ul className="nav nav-tabs mb-4">
        <li className="nav-item">
          <button
            className={`nav-link btn ${panel === 'add' ? 'active' : ''}`}
            onClick={() => setPanel(panel === 'add' ? '' : 'add')}
          >
            {panel === 'add' ? '✔️ Gotovo sa izmenama' : '➕ Dodaj kartu'}
          </button>
        </li>

        <li className="nav-item">
          <button
            className={`nav-link btn ${panel === 'edit' ? 'active' : ''}`}
            onClick={() => setPanel(panel === 'edit' ? '' : 'edit')}
          >
            🛠 Izmena karata
          </button>
        </li>
      </ul>

      {panel === 'add' && <AddTicketPage />}
      {panel === 'edit' && <EditTicketsPage />}
    </div>
  )
}

export default AdminDashboard
