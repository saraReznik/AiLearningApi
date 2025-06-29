import React, { createContext, useState, useEffect, useContext } from 'react';

// יצירת הקונטקסט
const AuthContext = createContext(undefined);

// ספק הקונטקסט
export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem('jwtToken'));
  const [isAuthenticated, setIsAuthenticated] = useState(!!token);

  // אתחול המשתמש אם יש token קיים
  useEffect(() => {
    const storedToken = localStorage.getItem('jwtToken');
    if (storedToken && !token) {
      setToken(storedToken);
      setIsAuthenticated(true);
      // כאן ניתן להוסיף שליפת משתמש מתוך הטוקן או API בעתיד
    }
  }, []);

  // עדכון סטטוס אימות בכל פעם שהטוקן משתנה
  useEffect(() => {
    setIsAuthenticated(!!token);
  }, [token]);

  // פונקציית התחברות
  const login = (newToken, userData = null) => {
    localStorage.setItem('jwtToken', newToken);
    setToken(newToken);
    setIsAuthenticated(true);
    if (userData) {
      setUser(userData);
    }
    console.log("התחברות הצליחה:", { newToken, userData });
  };

  // פונקציית התנתקות
  const logout = () => {
    localStorage.removeItem('jwtToken');
    setToken(null);
    setUser(null);
    setIsAuthenticated(false);
    console.log("התנתקות בוצעה");
  };

  // לוגים לאבחון (ניתן להסיר אחרי בדיקה)
  useEffect(() => {
    console.log("AuthContext:", { user, token, isAuthenticated });
  }, [user, token, isAuthenticated]);

  return (
    <AuthContext.Provider value={{ user, token, isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// הוק מותאם לשימוש נוח
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
