// src/pages/AdminPage.jsx
import React, { useState, useEffect } from 'react';
import { api } from '../api/axiosInstance';

// ייבוא רכיבי Material-UI
import {
  Container,
  Box,
  Typography,
  CircularProgress,
  Alert,
  Paper,
  Grid,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Card,
  CardContent,
} from '@mui/material';

const AdminPage = () => {
  const [users, setUsers] = useState([]);
  const [selectedUserPrompts, setSelectedUserPrompts] = useState([]);
  const [selectedUserId, setSelectedUserId] = useState(''); // שינוי לריק עבור Select
  const [loadingUsers, setLoadingUsers] = useState(false);
  const [loadingPrompts, setLoadingPrompts] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchAllUsers();
  }, []);

  useEffect(() => {
    if (selectedUserId) {
      fetchPromptsForUser(selectedUserId);
    } else {
      setSelectedUserPrompts([]);
    }
  }, [selectedUserId]);

  const fetchAllUsers = async () => {
    try {
      setLoadingUsers(true);
      const response = await api.getAllUsers();
      setUsers(response.data);
    } catch (err) {
      console.error('Failed to fetch all users:', err);
      setError('Failed to load users for admin dashboard.');
    } finally {
      setLoadingUsers(false);
    }
  };

  const fetchPromptsForUser = async (userId) => {
    try {
      setLoadingPrompts(true);
      const response = await api.getUserPrompts(userId);
      setSelectedUserPrompts(response.data);
    } catch (err) {
      console.error(`Failed to fetch prompts for user ${userId}:`, err);
      setError('Failed to load prompts for selected user.');
    } finally {
      setLoadingPrompts(false);
    }
  };

  return (
    <Container component="main" maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography component="h1" variant="h4" gutterBottom>
        פאנל ניהול
      </Typography>

      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      <Grid container spacing={4}>
        {/* קטע ניהול משתמשים */}
        <Grid item xs={12} md={6}>
          <Paper elevation={3} sx={{ p: 3 }}>
            <Typography variant="h5" component="h2" gutterBottom>
              כל המשתמשים
            </Typography>
            {loadingUsers ? (
              <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
                <CircularProgress />
              </Box>
            ) : users.length === 0 ? (
              <Typography variant="body2" color="text.secondary">
                אין משתמשים במערכת.
              </Typography>
            ) : (
              <FormControl fullWidth margin="normal">
                <InputLabel id="user-select-label">בחר/י משתמש</InputLabel>
                <Select
                  labelId="user-select-label"
                  id="user-select"
                  value={selectedUserId}
                  label="בחר/י משתמש"
                  onChange={(e) => setSelectedUserId(e.target.value)}
                >
                  <MenuItem value="">
                    <em>בחר/י משתמש כדי לראות היסטוריה</em>
                  </MenuItem>
                  {users.map((user) => (
                    <MenuItem key={user.userId} value={user.userId}> {/* שימוש ב-user.userId */}
                      {user.name} ({user.email}) - {user.phone}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            )}

            {/* טבלה להצגת רשימת המשתמשים (אופציונלי, אם רוצים להציג את כולם בטבלה ולא רק ב-select) */}
            {users.length > 0 && !selectedUserId && ( // הצג טבלה רק אם יש משתמשים ולא נבחר אחד ספציפי
              <TableContainer component={Paper} elevation={1} sx={{ mt: 3 }}>
                <Table size="small" aria-label="users table">
                  <TableHead>
                    <TableRow>
                      <TableCell>שם</TableCell>
                      <TableCell>אימייל</TableCell>
                      <TableCell>טלפון</TableCell>
                      <TableCell>תפקיד</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {users.map((user) => (
                      <TableRow key={user.userId}>
                        <TableCell>{user.name}</TableCell>
                        <TableCell>{user.email}</TableCell>
                        <TableCell>{user.phone}</TableCell>
                        <TableCell>{user.role}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            )}
          </Paper>
        </Grid>

        {/* קטע היסטוריית למידה למשתמש נבחר */}
        <Grid item xs={12} md={6}>
          <Paper elevation={3} sx={{ p: 3 }}>
            <Typography variant="h5" component="h2" gutterBottom>
              היסטוריית למידה עבור משתמש: {selectedUserId || 'לא נבחר'}
            </Typography>
            {loadingPrompts ? (
              <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
                <CircularProgress />
              </Box>
            ) : selectedUserId && selectedUserPrompts.length === 0 ? (
              <Typography variant="body2" color="text.secondary">
                למשתמש זה אין היסטוריית למידה.
              </Typography>
            ) : selectedUserPrompts.length === 0 ? (
                <Typography variant="body2" color="text.secondary">
                  בחר/י משתמש כדי להציג את היסטוריית הלמידה שלו.
                </Typography>
            ) : (
              <Box sx={{ maxHeight: 500, overflowY: 'auto' }}>
                {selectedUserPrompts.map((p) => (
                  <Card key={p.id} variant="outlined" sx={{ mb: 2 }}>
                    <CardContent>
                      <Typography variant="h6" component="div">
                        {p.prompt}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        {p.response}
                      </Typography>
                      <Typography variant="caption" display="block" sx={{ mt: 1 }}>
                        בתאריך: {new Date(p.createdAt).toLocaleString()}
                      </Typography>
                    </CardContent>
                  </Card>
                ))}
              </Box>
            )}
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default AdminPage;