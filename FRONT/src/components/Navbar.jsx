import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

function Navbar() {
  const { user, logout } = useAuth();
  const navigate         = useNavigate();

  const isAdmin = user?.roles?.includes('Admin');   // pretpostavljamo da je roles niz

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light px-4">
      <Link className="navbar-brand" to="/">Home</Link>
      <Link className="navbar-brand" to="/events">Events</Link>

      <div className="ms-auto d-flex align-items-center gap-3">

        {/* Admin vidi samo Admin panel */}
        {isAdmin && (
          <NavLink
            to="/admin"
            className={({ isActive }) =>
              'btn btn-outline-secondary' + (isActive ? ' active' : '')
            }
          >
            Admin panel
          </NavLink>
        )}

        {/* Ulogovani korisnik (ko NIJE admin) vidi Moj profil */}
        {user && !isAdmin && (
          <NavLink
            to="/profile"
            className={({ isActive }) =>
              'btn btn-outline-primary' + (isActive ? ' active' : '')
            }
          >
            Moj profil
          </NavLink>
        )}

        {/* Desno – pozdrav i login/logout dugmad */}
        {user ? (
          <>
            <span className="me-2">Hello, {user.firstName}</span>
            <button className="btn btn-outline-danger" onClick={handleLogout}>
              Logout
            </button>
          </>
        ) : (
          <>
            <Link className="btn btn-outline-primary" to="/login">Login</Link>
            <Link className="btn btn-outline-success" to="/register">Register</Link>
          </>
        )}
      </div>
    </nav>
  );
}

export default Navbar;
