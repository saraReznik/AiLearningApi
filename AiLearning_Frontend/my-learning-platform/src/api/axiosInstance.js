// src/api/axiosInstance.js
// אנחנו נשתמש ב-fetch API ישירות לצורך בדיקה במקום axios
// import axios from 'axios'; // השאר את זה כהערה

const API_BASE_URL = 'https://localhost:7120/api'; // <-- ודא שזו הכתובת הנכונה (HTTPS)

// פונקציה עזר לביצוע קריאות fetch עם טיפול ב-JWT
async function authorizedFetch(url, options = {}) {
  const token = localStorage.getItem('jwtToken');
  const headers = {
    'Content-Type': 'application/json',
    ...options.headers,
  };

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  const response = await fetch(`${API_BASE_URL}${url}`, {
    ...options,
    headers,
  });

  // אם התגובה אינה בסדר (לדוגמה, סטטוס 4xx או 5xx)
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({ message: 'שגיאה לא ידועה' }));
    throw new Error(errorData.message || `שגיאת רשת: ${response.status}`);
  }

  return response.json(); // החזר את נתוני ה-JSON של התגובה
}

export const api = {
  // Auth & User
  login: (credentials) => authorizedFetch('/Auth/login', { method: 'POST', body: JSON.stringify(credentials) }),
  register: (userData) => authorizedFetch('/User', { method: 'POST', body: JSON.stringify(userData) }),
  getAllUsers: () => authorizedFetch('/User/admin/all'),

  // Categories
  getCategories: () => authorizedFetch('/Category'),
  getSubCategories: (categoryId) => authorizedFetch(`/SubCategory/byCategory/${categoryId}`),

  // Prompts
  submitPrompt: (promptData) => authorizedFetch('/Prompt', { method: 'POST', body: JSON.stringify(promptData) }),
  getUserPrompts: (userId) => authorizedFetch(`/Prompt/user/${userId}`),
  getAllPrompts: () => authorizedFetch('/Prompt/admin/all'),
};