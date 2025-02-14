import React, { useState } from 'react';
import { createExpense, updateExpense } from '../services/api';
import './ExpenseForm.css';

interface ExpenseFormProps {
  expense?: any;
  onSave: (newExpense: any) => void;
}

const ExpenseForm: React.FC<ExpenseFormProps> = ({ expense, onSave }) => {
  const [formData, setFormData] = useState({
    date: expense?.date || '',
    amount: expense?.amount || '',
    description: expense?.description || '',
    category: expense?.category || '',
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    let newExpense;
    if (expense) {
      newExpense = await updateExpense(expense.id, formData);
    } else {
      newExpense = await createExpense(formData);
    }
    onSave(newExpense);
  };

  return (
    <form className="expense-form" onSubmit={handleSubmit}>
      <div>
        <label>Date</label>
        <input type="date" name="date" value={formData.date} onChange={handleChange} />
      </div>
      <div>
        <label>Amount</label>
        <input type="number" name="amount" value={formData.amount} onChange={handleChange} />
      </div>
      <div>
        <label>Description</label>
        <input type="text" name="description" value={formData.description} onChange={handleChange} />
      </div>
      <div>
        <label>Category</label>
        <input type="text" name="category" value={formData.category} onChange={handleChange} />
      </div>
      <button type="submit">Save</button>
    </form>
  );
};

export default ExpenseForm;