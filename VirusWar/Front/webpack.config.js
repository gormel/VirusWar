const path = require('path');

module.exports = {
  entry: './src/index.ts',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'main.bundle.js',
  },
  module: {
    rules: [
      {test: /\.css$/, use: 'css-loader'},
      {test: /\.ts$/, use: 'ts-loader'},
    ]
  },
};
