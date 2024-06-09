import React, { useEffect, useState } from 'react';
import api from '../api';
import '../styles/ArtifactsPage.css';

const ArtifactsPage = () => {
  const [artifacts, setArtifacts] = useState([]);

  useEffect(() => {
    const fetchArtifacts = async () => {
      try {
        const response = await api.get('/artifacts');
        setArtifacts(response.data);
      } catch (error) {
        console.error('Error fetching artifacts:', error);
      }
    };

    fetchArtifacts();
  }, []);

  return (
    <div className="artifacts-page">
      <h1>Artifacts</h1>
      <ul>
        {artifacts.map(artifact => (
          <li key={artifact.id}>{artifact.title}</li>
        ))}
      </ul>
    </div>
  );
};

export default ArtifactsPage;
