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

  sessionIdText: string;

  render() {
    const {session, playerId, creatingSession} = this.state;
    if (session && playerId) {
      return <Redirect to={`/game/${session.id}/player/${playerId}`} />;
    }

    return (
      <div style={{display: "flex", flexDirection: "column"}}>
        <button type='button'
          disabled={creatingSession}
          onClick={this.onCreateGameClick}>
          Create new game
        </button>
        <input type="text" onChange={(e) => this.sessionIdText = e.target.value}/>
        <button onClick={this.onConnectClick}>Connect to game</button>
      </div>
    );
  }

  private onCreateGameClick = async () => {
    this.setState({creatingSession: true});
    const session = await createSession();
    const player = await session.join();
    this.setState({creatingSession: false, session, playerId: player.id});
  }

  private onConnectClick = async () => {
    this.setState({creatingSession: true});
    const session = new GameSession(this.sessionIdText);
    const player = await session.join();
    if (player == null)
      this.setState({creatingSession: false});
    else
      this.setState({creatingSession: false, session, playerId: player.id});
  }
}
