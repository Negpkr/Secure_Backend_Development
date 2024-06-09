import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import NavigationBar from './components/NavigationBar';
import HomePage from './components/HomePage';
import ArtistsPage from './components/ArtistsPage';
import ArtifactsPage from './components/ArtifactsPage';
import ExhibitionsPage from './components/ExhibitionsPage';

const App = () => {
  return (
    <Router>
      <NavigationBar />
      <Routes>
        <Route path="/" exact component={HomePage} />
        <Route path="/artists" component={ArtistsPage} />
        <Route path="/artifacts" component={ArtifactsPage} />
        <Route path="/exhibitions" component={ExhibitionsPage} />
      </Routes>
    </Router>
  );
};

export default App;
