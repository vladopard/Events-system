import { BrowserRouter, Routes, Route } from 'react-router-dom';
import EventsPage from './pages/EventsPage';
import EventDetailsPage from './pages/EventDetailsPage';
import HomePage from './pages/HomePage';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/events" element={<EventsPage />} />
        <Route path='/events/:id' element={<EventDetailsPage />} />
        <Route path='/' element={<HomePage />} />
      </Routes>
    </BrowserRouter>
  )
}