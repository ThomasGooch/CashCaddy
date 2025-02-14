// filepath: /Users/thomas.gooch/Projects/POC/CashCaddy/frontend/cash-caddy-ui/src/api.ts

import axios from 'axios';

// const backendUrl = process.env.REACT_APP_BACKEND_URL;
const backendUrl = "http://localhost:5000"

export const getExpenses = async () => {
  const response = await axios.get(`${backendUrl}/expenses`);
  return response.data;
};

export const getExpenseById = async (id: string) => {
  const response = await axios.get(`${backendUrl}/expenses/${id}`);
  return response.data;
};

export const createExpense = async (expense: any) => {
  const response = await axios.post(`${backendUrl}/expenses`, expense);
  return response.data;
};

export const updateExpense = async (id: string, expense: any) => {
  const response = await axios.put(`${backendUrl}/expenses/${id}`, expense);
  return response.data;
};

export const deleteExpense = async (id: string) => {
  const response = await axios.delete(`${backendUrl}/expenses/${id}`);
  return response.data;
};