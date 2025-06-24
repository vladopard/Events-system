import { createContext, useState, useContext, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode'

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);

    // pri mountu učitaj iz localStorage
    useEffect(() => {
        const token = localStorage.getItem('token')
        if (token) {
            const decoded = jwtDecode(token)
            setUser({
                token,
                userId: decoded.sub,
                email: decoded.email,
                firstName: decoded.firstName,
                roles: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
            })
        }
    }, [])

    const login = (data) => {
        const decoded = jwtDecode(data.token)

        const userData = {
            token: data.token,
            email: decoded.email,
            firstName: decoded.firstName,
            roles: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
            userId: decoded.sub, // ← ovo je bitno za profile
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