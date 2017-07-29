const path = require('path');

module.exports = {
  entry: './src/index.ts',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'main.bundle.js',
  },
  resolve: {
    extensions: ['.js', '.ts', '.tsx'],
  },
  module: {
    rules: [
      {test: /\.css$/, use: 'css-loader'},
      {test: /\.tsx?$/, use: 'ts-loader'},
    ]
  },
};
