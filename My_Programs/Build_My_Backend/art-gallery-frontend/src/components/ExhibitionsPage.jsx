import React, { useEffect, useState } from 'react';
import api from '../api';
import '../styles/ExhibitionsPage.css';

const ExhibitionsPage = () => {
  const [exhibitions, setExhibitions] = useState([]);

  useEffect(() => {
    const fetchExhibitions = async () => {
      try {
        const response = await api.get('/exhibitions');
        setExhibitions(response.data);
      } catch (error) {
        console.error('Error fetching exhibitions:', error);
      }
    };

    fetchExhibitions();
  }, []);

  return (
    <div className="exhibitions-page">
      <h1>Exhibitions</h1>
      <ul>
        {exhibitions.map(exhibition => (
          <li key={exhibition.id}>{exhibition.name}</li>
        ))}
      </ul>
    </div>
  );
};

export default ExhibitionsPage;
