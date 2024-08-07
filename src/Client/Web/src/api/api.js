import axios from 'axios';

const API_URL = 'http://localhost:3000'; 

export const getAllProducts = async () => {
  try {
    const response = await axios.get(`${API_URL}/products`);
    console.log('...resssss1', response);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getUsers = async () => {
  try {
    const response = await axios.get(`${API_URL}/users`);
    console.log('...resssss2', response);
    return response.data;
  } catch (error) {
    throw error;
  }
};
