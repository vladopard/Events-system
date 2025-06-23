import { createContext, useState, useContext } from 'react';
import { jwtDecode } from 'jwt-decode'

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);

    const login = (data) => {
        const decoded = jwtDecode(data.token)

        const userData = {
            token: data.token,
            ...decoded
        }

        setUser(userData)
        localStorage.setItem('token', data.token)
    }

    const logout = () => {
        setUser(null)
        localStorage.removeItem('token');
    }

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => useContext(AuthContext);