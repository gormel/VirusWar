import * as React from 'react';
import { Component } from 'react';

export interface GameProps {
  readonly sessionId: string;
  readonly playerId: string;
}

export class Game extends Component<GameProps, {}> {
  render() {
    const {sessionId, playerId} = this.props;
    return <div>Game {sessionId} as player {playerId} </div>;
  }
}
