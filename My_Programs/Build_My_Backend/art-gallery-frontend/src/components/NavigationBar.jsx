import React from 'react';
import '../styles/NavigationBar.css';

const NavigationBar = () => {
  return (
    <div className="navbar">
      <div className="menu">
        <a href="/">Home</a>
        <a href="/artists">Artists</a>
        <a href="/artifacts">Artifacts</a>
        <a href="/exhibitions">Exhibitions</a>
      </div>
      <p>Art Gallery</p>
    </div>
  );
};

export default NavigationBar;

