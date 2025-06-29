// src/components/Auth/Login.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { api } from '../../api/axiosInstance';

// ייבוא רכיבי Material-UI
import {
  Container,
  Box,
  TextField,
  Button,
  Typography,
  Alert // להודעות שגיאה בצורה יפה
} from '@mui/material';
import { LockOutlined as LockOutlinedIcon } from '@mui/icons-material'; // אייקון של מנעול

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); // מאפס את הודעת השגיאה לפני כל ניסיון חדש

    try {
      const response = await api.login({ email, phone });
      const { token, user } = response.data;
      console.log("Login API response:", response.data);

      login(token, user);
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.message || 'התחברות נכשלה. אנא בדוק פרטים.');
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
        <LockOutlinedIcon color="primary" sx={{ fontSize: 40, mb: 2 }} />
        <Typography component="h1" variant="h5" sx={{ mb: 3 }}>
          התחברות
        </Typography>
        <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="אימייל"
            name="email"
            autoComplete="email"
            autoFocus
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="phone"
            label="טלפון"
            type="tel" // סוג טלפון לוולידציה בסיסית
            id="phone"
            autoComplete="current-phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
          />
          {error && (
            <Alert severity="error" sx={{ mt: 2, width: '100%' }}>
              {error}
            </Alert>
          )}
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            התחבר
          </Button>
          {/* כאן נוכל להוסיף לינק להרשמה בעתיד */}
        </Box>
      </Box>
    </Container>
  );
};

export default LoginPage;