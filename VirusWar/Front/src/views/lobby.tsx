import * as React from 'react';
import { Component } from 'react';
import { Redirect } from 'react-router-dom';

import { GameSession, createSession } from '../service';

export interface LobbyProps {}

interface State {
  readonly creatingSession?: boolean;
  readonly session?: GameSession;
  readonly playerId?: string;
}

export class Lobby extends Component<LobbyProps, State> {
  constructor(props: LobbyProps, context: any) {
    super(props, context);
    this.state = {};
  }

  render() {
    const {session, playerId, creatingSession} = this.state;
    if (session && playerId) {
      return <Redirect to={`/game/${session.id}/player/${playerId}`} />;
    }

    return (
      <button type='button'
        disabled={creatingSession}
        onClick={this.onCreateGameClick}>
        Create new game
      </button>
    );
  }

  private onCreateGameClick = async () => {
    this.setState({creatingSession: true});
    const session = await createSession();
    const player = await session.join();
    this.setState({creatingSession: false, session, playerId: player.id});
  }
}
