// src/components/Auth/Register.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { api } from '../../api/axiosInstance';

// ייבוא רכיבי Material-UI
import {
  Container,
  Box,
  TextField,
  Button,
  Typography,
  Alert,
  Link // להוספת לינק להתחברות
} from '@mui/material';
import { PersonAddOutlined as PersonAddOutlinedIcon } from '@mui/icons-material'; // אייקון של הוספת אדם

const RegisterPage = () => {
  const [name, setName] = useState('');
  const [phone, setPhone] = useState('');
  const [email, setEmail] = useState('');
  // אין שדה סיסמה כאן, בהתאם ל-BLUser ולמה שאמרת קודם
  const [role, setRole] = useState('User'); // נניח שזה קבוע "User"

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    try {
      const userDataToSend = {
          userId: 0, // השרת אמור לייצר ID, נשלח 0 כברירת מחדל
          name,
          phone,
          email,
          role // נשלח את ה-Role כפי שהוא מוגדר ב-BLUser
      };

      await api.register(userDataToSend);

      setSuccess('נרשמת בהצלחה! כעת תוכל להתחבר.');
      // איפוס שדות הטופס לאחר רישום מוצלח
      setName('');
      setPhone('');
      setEmail('');
      setRole('User'); // איפוס גם את ה-role
      setTimeout(() => navigate('/login'), 2000); // ניווט לדף ההתחברות
    } catch (err) {
      setError(err.response?.data?.message || 'הרשמה נכשלה. אנא נסה שוב.');
    }
  };

  return (
    <Container component="main" maxWidth="xs" sx={{ mt: 8 }}>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          p: 3,
          boxShadow: 3,
          borderRadius: 2,
          bgcolor: 'background.paper',
        }}
      >
        <PersonAddOutlinedIcon color="primary" sx={{ fontSize: 40, mb: 2 }} />
        <Typography component="h1" variant="h5" sx={{ mb: 3 }}>
          הרשמה
        </Typography>
        <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="name"
            label="שם"
            name="name"
            autoComplete="name"
            autoFocus
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="אימייל"
            name="email"
            autoComplete="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="phone"
            label="טלפון"
            type="tel"
            id="phone"
            autoComplete="phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
          />
          {/* שדה סיסמה לא נכלל כאן בטופס אם הוא לא ב-BLUser ולא נדרש לרישום */}
          {error && (
            <Alert severity="error" sx={{ mt: 2, width: '100%' }}>
              {error}
            </Alert>
          )}
          {success && (
            <Alert severity="success" sx={{ mt: 2, width: '100%' }}>
              {success}
            </Alert>
          )}
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            הירשם
          </Button>
          <Link href="/login" variant="body2">
            כבר יש לך חשבון? התחבר/י
          </Link>
        </Box>
      </Box>
    </Container>
  );
};

export default RegisterPage;