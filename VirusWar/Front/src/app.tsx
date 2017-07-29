import * as React from 'react';
import { Component } from 'react';
import { Router, Route } from 'react-router-dom';
import createBrowserHistory from 'history/createBrowserHistory';

import { Lobby } from './views/lobby';
import { Game } from './views/game';

const history = createBrowserHistory();

export class App extends Component<{}, {}> {
  render() {
    return (
      <Router history={history}>
        <div>
          <Route path='/' component={Lobby} />
          <Route path='/game/:sessionId/player/:playerId' component={Game} />
        </div>
      </Router>
    );
  }
}
