import React, { Component } from 'react';

export class Home extends Component {
  displayName = Home.name

  render() {
    return (
      <div>
        <h1>Stack Overflow Tags</h1>
        <p>To discover 100 most popular tags, press 'Fetch data'</p>
      </div>
    );
  }
}
