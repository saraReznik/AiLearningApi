// src/pages/DashboardPage.jsx
import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { api } from '../api/axiosInstance';

// ייבוא רכיבי Material-UI
import {
  Container,
  Box,
  TextField,
  Button,
  Typography,
  CircularProgress,
  Alert,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Paper,
  Card,
  CardContent,
  Grid // עבור פריסה רספונסיבית
} from '@mui/material';

const DashboardPage = () => {
  const { user } = useAuth();
  const [categories, setCategories] = useState([]);
  const [subCategories, setSubCategories] = useState([]);
  const [selectedCategoryId, setSelectedCategoryId] = useState(''); // שינוי לריק עבור Select
  const [selectedSubCategoryId, setSelectedSubCategoryId] = useState(''); // שינוי לריק עבור Select
  const [promptText, setPromptText] = useState('');
  const [aiResponse, setAiResponse] = useState('');
  const [userPrompts, setUserPrompts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // טעינת קטגוריות והיסטוריית משתמש בעת טעינת הרכיב
  useEffect(() => {
    fetchCategories();
    // UserId אמור להיות זמין כעת לאחר התחברות
    if (user?.userId) { // שימוש ב-user.userId כפי שחוזר מהבקאנד
      fetchUserPrompts(user.userId);
    } else {
        // Fallback: אם user.userId עדיין לא זמין מיד מהקונטקסט
        const token = localStorage.getItem('jwtToken');
        if (token) {
            try {
                const base64Url = token.split('.')[1];
                const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
                const decodedToken = JSON.parse(window.atob(base64));
                const userId = decodedToken.id || decodedToken.sub || decodedToken.userId; // Try various keys
                if (userId) {
                    fetchUserPrompts(userId);
                }
            } catch (e) {
                console.error("Failed to decode token:", e);
            }
        }
    }
  }, [user?.userId]); // טען מחדש כשה-user.userId משתנה

  // טעינת תת-קטגוריות כאשר קטגוריה נבחרת
  useEffect(() => {
    if (selectedCategoryId) {
      fetchSubCategories(selectedCategoryId);
    } else {
      setSubCategories([]);
      setSelectedSubCategoryId(''); // איפוס תת-קטגוריה נבחרת
    }
  }, [selectedCategoryId]);

  const fetchCategories = async () => {
    try {
      const response = await api.getCategories();
      setCategories(response.data);
    } catch (err) {
      console.error('Failed to fetch categories:', err);
      setError('Failed to load categories.');
    }
  };

  const fetchSubCategories = async (categoryId) => {
    try {
      const response = await api.getSubCategories(categoryId);
      setSubCategories(response.data);
    } catch (err) {
      console.error('Failed to fetch subcategories:', err);
      setError('Failed to load subcategories.');
    }
  };

  const fetchUserPrompts = async (userId) => {
    try {
      setLoading(true);
      const response = await api.getUserPrompts(userId);
      setUserPrompts(response.data);
    } catch (err) {
      console.error('Failed to fetch user prompts:', err);
      setError('Failed to load learning history.');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmitPrompt = async (e) => {
    e.preventDefault();
    setError('');
    setAiResponse('');

    let currentUserId = user?.userId; // שימוש ב-user.userId
    if (!currentUserId) {
        // Fallback: אם user.userId עדיין לא זמין מיד מהקונטקסט
        const token = localStorage.getItem('jwtToken');
        if (token) {
            try {
                const base64Url = token.split('.')[1];
                const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
                const decodedToken = JSON.parse(window.atob(base64));
                currentUserId = decodedToken.id || decodedToken.sub || decodedToken.userId; // Try various keys
            } catch (e) {
                console.error("Failed to decode token for prompt submission:", e);
            }
        }
    }


    if (!currentUserId || !selectedCategoryId || !selectedSubCategoryId || !promptText) {
      setError('אנא מלא/י את כל השדות.');
      return;
    }

    try {
      setLoading(true);
      const promptData = {
        userId: currentUserId,
        categoryId: selectedCategoryId,
        subCategoryId: selectedSubCategoryId,
        prompt: promptText,
      };
      const response = await api.submitPrompt(promptData);
      setAiResponse(response.data.response);
      setPromptText('');
      fetchUserPrompts(currentUserId);
    } catch (err) {
      console.error('Failed to submit prompt:', err);
      setError(err.response?.data?.message || 'שגיאה בקבלת תגובת AI.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container component="main" maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography component="h1" variant="h4" gutterBottom>
        ברוך הבא/ה ללומדה, {user?.name || 'משתמש/ת'}!
      </Typography>

      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      <Grid container spacing={4}>
        {/* קטע קבלת שיעור חדש */}
        <Grid item xs={12} md={6}>
          <Paper elevation={3} sx={{ p: 3 }}>
            <Typography variant="h5" component="h2" gutterBottom>
              קבל שיעור חדש
            </Typography>
            <Box component="form" onSubmit={handleSubmitPrompt} noValidate>
              <FormControl fullWidth margin="normal">
                <InputLabel id="category-label">קטגוריה</InputLabel>
                <Select
                  labelId="category-label"
                  id="category-select"
                  value={selectedCategoryId}
                  label="קטגוריה"
                  onChange={(e) => setSelectedCategoryId(e.target.value)}
                >
                  <MenuItem value="">
                    <em>בחר/י קטגוריה</em>
                  </MenuItem>
                  {categories.map((cat) => (
                    <MenuItem key={cat.id} value={cat.id}>{cat.name}</MenuItem>
                  ))}
                </Select>
              </FormControl>

              <FormControl fullWidth margin="normal">
                <InputLabel id="subCategory-label">תת-קטגוריה</InputLabel>
                <Select
                  labelId="subCategory-label"
                  id="subCategory-select"
                  value={selectedSubCategoryId}
                  label="תת-קטגוריה"
                  onChange={(e) => setSelectedSubCategoryId(e.target.value)}
                  disabled={!selectedCategoryId || subCategories.length === 0}
                >
                  <MenuItem value="">
                    <em>בחר/י תת-קטגוריה</em>
                  </MenuItem>
                  {subCategories.map((subCat) => (
                    <MenuItem key={subCat.id} value={subCat.id}>{subCat.name}</MenuItem>
                  ))}
                </Select>
              </FormControl>

              <TextField
                margin="normal"
                required
                fullWidth
                id="promptText"
                label="שאלתך ל-AI"
                multiline
                rows={4}
                placeholder="לדוגמה: למד/י אותי על חורים שחורים."
                value={promptText}
                onChange={(e) => setPromptText(e.target.value)}
              />
              <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 2, mb: 2 }}
                disabled={loading}
              >
                {loading ? <CircularProgress size={24} /> : 'קבל שיעור'}
              </Button>
            </Box>

            {aiResponse && (
              <Box sx={{ mt: 3, p: 2, border: '1px dashed grey', borderRadius: 1 }}>
                <Typography variant="h6" gutterBottom>
                  תשובת ה-AI:
                </Typography>
                <Typography variant="body1">
                  {aiResponse}
                </Typography>
              </Box>
            )}
          </Paper>
        </Grid>

        {/* קטע היסטוריית למידה */}
        <Grid item xs={12} md={6}>
          <Paper elevation={3} sx={{ p: 3 }}>
            <Typography variant="h5" component="h2" gutterBottom>
              היסטוריית למידה
            </Typography>
            {loading && userPrompts.length === 0 ? (
              <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
                <CircularProgress />
              </Box>
            ) : userPrompts.length === 0 ? (
              <Typography variant="body2" color="text.secondary">
                אין היסטוריית למידה להצגה. התחל/י ללמוד!
              </Typography>
            ) : (
              <Box sx={{ maxHeight: 500, overflowY: 'auto' }}>
                {userPrompts.map((p) => (
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

export default DashboardPage;