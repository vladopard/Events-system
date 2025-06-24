import { NavLink, Routes, Route, Navigate } from 'react-router-dom'
import AddTicketPage from './AddTicketPage'

function AdminDashboard() {
  return (
    <div className="container mt-4">
      <h2 className="mb-4">Admin panel</h2>

      <ul className="nav nav-tabs mb-4">
        <li className="nav-item">
          <NavLink className="nav-link" to="add-ticket">
            ➕ Dodaj kartu
          </NavLink>
        </li>
        {/* Dalje tabove (events, stats …) dodaćeš kasnije */}
      </ul>

      <Routes>
        <Route path="add-ticket" element={<AddTicketPage />} />
        <Route path="*" element={<Navigate to="add-ticket" replace />} />
      </Routes>
    </div>
  )
}

export default AdminDashboard
