// src/pages/AdminDashboard.jsx
import { useState } from 'react'
import { BsPlus, BsWrench, BsTag } from 'react-icons/bs'

import AddTicketPage from './AddTicketPage'
import EditTicketsPage from './EditTicketsPage'
import AddTicketTypePage from './AddTicketTypePage'

export default function AdminDashboard() {
  // активни таб: addTicket | editTickets | addTicketType
  const [tab, setTab] = useState('addTicket')

  // унутрашња компонента за линкове
  const NavLink = ({ id, icon: Icon, children }) => (
    <button
      onClick={() => setTab(id)}
      className={`btn btn-link p-0 me-4 ${
        tab === id ? 'fw-bold text-primary' : 'text-decoration-none'
      }`}
    >
      <Icon className="me-1" />
      {children}
    </button>
  )

  return (
    <div className="container mt-4">
      <h1>Admin Dashboard</h1>

      {/* таб навигација */}
      <div className="d-flex align-items-center mb-3">
        <NavLink id="addTicket" icon={BsPlus}>
          Dodaj kartu
        </NavLink>

        <NavLink id="editTickets" icon={BsWrench}>
          Izmena karata
        </NavLink>

        <NavLink id="addTicketType" icon={BsTag}>
          Dodaj tip karte
        </NavLink>
      </div>
      <hr />

      {/* садржај по табу */}
      {tab === 'addTicket' && <AddTicketPage />}
      {tab === 'editTickets' && <EditTicketsPage />}
      {tab === 'addTicketType' && <AddTicketTypePage />}
    </div>
  )
}
