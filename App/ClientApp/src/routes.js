import React from 'react';
import { Route } from 'react-router-dom';
import Configuration from './configuration/Configuration';
import Segments from './segments/Segments';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

export default [
    <Route exact path='/' component={Home} />,
    <Route path='/counter' component={Counter} />,
    <Route path='/fetch-data/:startDateIndex?' component={FetchData} />,
    <Route exact path="/configuration" render={() => <Configuration />} />,
    <Route exact path="/segments" render={() => <Segments />} />,
];
