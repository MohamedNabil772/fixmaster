import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5266', // ApiGateway URL
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor for Authorization header
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  
  // Add Correlation ID
  const correlationId = crypto.randomUUID();
  config.headers['X-Correlation-Id'] = correlationId;
  
  return config;
});

export default api;
