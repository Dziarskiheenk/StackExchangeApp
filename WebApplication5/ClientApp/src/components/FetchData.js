import React, { Component } from 'react';
import { DropdownButton, MenuItem, Alert } from 'react-bootstrap';

export class FetchData extends Component {
  displayName = FetchData.name

  constructor(props) {
    super(props);
    this.state = { tags: [], loading: true, sortBy: 'popularity', error: undefined };

    this.fetchTags();
  }

  fetchTags = () => {
    fetch('api/StackExchange/Tags')
      .then(async response => {
        if (response.ok)
          return await response.json();
        else {
          let errorText = await response.text();
          throw new Error(errorText);
        }
      })
      .then(response => {
        this.setState({ tags: response, loading: false, sortBy: 'popularity' });
      })
      .catch(error => {
        this.setState({ tags: [], error: error, loading: false });
      });
  }

  renderTagsData = tags => {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Name</th>
            <th>Popularity</th>
            <th>Popularity [%]</th>
          </tr>
        </thead>
        <tbody>
          {tags.map((tag, index) =>
            <tr key={index}>
              <td>{tag.name}</td>
              <td>{tag.count}</td>
              <td>{tag.popularityPercentage}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  onSortChange = sortType => {
    debugger;
    let newTags = JSON.parse(JSON.stringify(this.state.tags));
    switch (sortType) {
      case 'popularity':
        newTags.sort((a, b) => (a.count > b.count) ? -1 : ((b.count > a.count) ? 1 : 0));
        break;
      case 'name':
        newTags.sort((a, b) => (a.name > b.name) ? 1 : ((b.name > a.name) ? -1 : 0));
        break;
    }
    this.setState({ tags: newTags, sortBy: sortType });
  }

  render() {
    return (
      <div>
        <h1>Stack Overflow Tags</h1>
        {this.state.error &&
          <Alert bsStyle='danger'>
            An error occured with following message:  {' ' + this.state.error}
          </Alert>}
        {!this.state.loading &&
          <DropdownButton
            title={'Sort'}
            key={1}
            id={`dropdown-basic-${1}`}>
            <MenuItem key={2} eventKey="popularity" active={this.state.sortBy === 'popularity'} onSelect={this.onSortChange}>Popularity</MenuItem>
            <MenuItem key={3} eventKey="name" active={this.state.sortBy === 'name'} onSelect={this.onSortChange}>Name</MenuItem>
          </DropdownButton>}
        {this.state.loading ?
          <p><em>Loading...</em></p> :
          this.renderTagsData(this.state.tags)}
      </div>
    );
  }
}
