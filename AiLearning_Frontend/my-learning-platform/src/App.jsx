import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import LoginPage from './components/Auth/Login';
import RegisterPage from './components/Auth/Register';
import DashboardPage from './pages/DashboardPage';
import AdminPage from './pages/AdminPage';

const PrivateRoute = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" />;
};

const AdminRoute = ({ children }) => {
  const { isAuthenticated /* , user */ } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" />;
};

const App = () => {
  return (
    <AuthProvider>
      <Router>
        <nav style={{ padding: '10px', borderBottom: '1px solid #ccc' }}>
          <ul style={{ listStyle: 'none', padding: 0, margin: 0, display: 'flex', gap: '15px' }}>
            <li><Link to="/">ראשי</Link></li>
            <li><Link to="/register">הרשמה</Link></li>
            <li><Link to="/login">התחברות</Link></li>
            <li><Link to="/admin">ניהול</Link></li>
            <AuthStatus />
          </ul>
        </nav>
        <div style={{ padding: '20px' }}>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/" element={<PrivateRoute><DashboardPage /></PrivateRoute>} />
            <Route path="/admin" element={<AdminRoute><AdminPage /></AdminRoute>} />
            <Route path="*" element={<h1>404 - לא נמצא</h1>} />
          </Routes>
        </div>
      </Router>
    </AuthProvider>
  );
};

const AuthStatus = () => {
  const { isAuthenticated, user, logout } = useAuth();
  return (
    <li style={{ marginLeft: 'auto' }}>
      {isAuthenticated ? (
        <>
          <span>שלום, {user?.name || 'משתמש'}!</span>
          <button onClick={logout} style={{ marginLeft: '10px' }}>התנתק</button>
        </>
      ) : (
        <span>לא מחובר</span>
      )}
    </li>
  );
};

export default App;