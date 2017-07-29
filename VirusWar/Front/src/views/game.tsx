import * as React from 'react';
import { Component } from 'react';
import { match } from 'react-router-dom';

export interface GameProps {
  readonly match: match<{
    readonly sessionId: string;
    readonly playerId: string;
  }>;
}

export class Game extends Component<GameProps, {}> {
  render() {
    const {sessionId, playerId} = this.props.match.params;
    return <div>Game {sessionId} as player {playerId} </div>;
  }
}
