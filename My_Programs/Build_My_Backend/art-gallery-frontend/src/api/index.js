import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5053/api', 
});

export default api;
