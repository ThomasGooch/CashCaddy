import React from 'react';
import { deleteExpense } from '../services/api';
import './Expenses.css';

interface ExpensesProps {
  expenses: any[];
  onDelete: (id: string) => void;
}

const Expenses: React.FC<ExpensesProps> = ({ expenses, onDelete }) => {
  const handleDelete = async (id: string) => {
    await deleteExpense(id);
    onDelete(id);
    // Optionally, you can call a callback to update the parent state
  };

  return (
    <div className="expenses-container">
      <h2>Expenses</h2>
      <ul className="expenses-list">
        {expenses.map((expense) => (
          <li key={expense.id}>
            {expense.description} - ${expense.amount}
            <button onClick={() => handleDelete(expense.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Expenses;