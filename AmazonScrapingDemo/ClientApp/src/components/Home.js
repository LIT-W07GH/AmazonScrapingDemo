import React, { Component } from 'react';
import axios from 'axios';

export class Home extends Component {
  displayName = Home.name

  state = {
    amazonItems: [],
    searchTerm: ''
  }

  onTextChange = e => this.setState({ searchTerm: e.target.value });

  onSearchClick = async () => {
    const { data } = await axios.get(`/api/amazon/search/${this.state.searchTerm}`);
    this.setState({ amazonItems: data });
  }

  render() {
    return (
      <div>
        <div className="row">
          <div className="col-md-10">
            <input type="text" className="input-lg form-control"
              placeholder="Search...." onChange={this.onTextChange} />
          </div>
          <div className="col-md-2">
            <button className="btn btn-success btn-lg btn-block" onClick={this.onSearchClick}>
              Search
            </button>
          </div>
        </div>
        <div className="row">
          <table className="table table-hover table-striped table-bordered">
            <thead>
              <tr>
                <th>Image</th>
                <th>Title</th>
                <th>Price</th>
              </tr>
            </thead>
            <tbody>
              {this.state.amazonItems.map((item, i) => {
                return (
                  <tr key={i}>
                    <td>
                      <img src={item.imageUrl} />
                    </td>
                    <td>
                      <a target="_blank" href={item.url}>
                        {item.title}
                      </a>
                    </td>
                    <td>
                      {item.price}
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        </div>
      </div>
    );
  }
}
