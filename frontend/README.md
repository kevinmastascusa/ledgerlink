# LedgerLink Frontend ⚛️

The frontend application for LedgerLink, built with React and Material-UI. This application provides a modern, responsive interface for managing financial accounts and transactions.

## ✨ Features

### 🔐 Authentication
- 🔑 JWT-based authentication
- 🛡️ Protected routes
- 👮 Role-based access control
- ⏱️ Session management
- 🔄 Password change functionality

### 📊 Dashboard
- 💳 Account overview
- 📜 Recent transactions
- 💰 Balance summaries
- ⚡ Quick actions
- 🔔 Notification center

### 💳 Account Management
- 📋 Account listing and details
- ➕ Account creation and editing
- 💰 Balance tracking
- 📜 Transaction history
- 📈 Account status monitoring

### 💸 Transaction Management
- 📋 Transaction listing and filtering
- ➕ Transaction creation and editing
- ⚡ Real-time balance updates
- 🏷️ Transaction categories
- 🔍 Search and filter capabilities

### 🔔 Notifications
- ⚡ Real-time notifications
- ⚙️ Notification preferences
- ✅ Read/unread status
- 📜 Notification history
- 🚨 System alerts

## 🛠️ Tech Stack

- ⚛️ React 18.x
- 🎨 Material-UI v5
- 📦 Redux Toolkit for state management
- 🛣️ React Router DOM v6
- 🌐 Axios for API communication
- 🧪 Jest and React Testing Library
- 📝 ESLint and Prettier

## 📁 Project Structure

```
frontend/
├── public/                 # 📂 Static files
├── src/
│   ├── assets/            # 🎨 Static assets
│   │   └── images/        # 🖼️ Images and logos
│   │       ├── logo.svg   # 📝 Main logo
│   │       └── logo-icon.svg # 🔷 Icon version
│   ├── components/        # 🧩 Reusable components
│   │   ├── common/       # 🔄 Shared components
│   │   ├── layout/       # 📐 Layout components
│   │   └── features/     # ⚡ Feature-specific components
│   ├── pages/            # 📄 Page components
│   ├── services/         # 🌐 API services
│   ├── store/            # 📦 Redux store
│   │   ├── slices/       # 🍕 Redux slices
│   │   └── hooks.ts      # 🎣 Redux hooks
│   ├── utils/            # 🛠️ Utility functions
│   ├── types/            # 📝 TypeScript types
│   ├── hooks/            # 🎣 Custom hooks
│   ├── context/          # 🔄 React context
│   └── theme/            # 🎨 Material-UI theme
└── tests/                # 🧪 Test files
```

## 🚀 Getting Started

### 📋 Prerequisites
- 🟢 Node.js 16.x or later
- 📦 npm 8.x or later

### ⚙️ Installation
1. Install dependencies:
   ```bash
   npm install
   ```

2. Create a `.env` file in the root directory:
   ```
   REACT_APP_API_URL=http://localhost:5000
   ```

3. Start the development server:
   ```bash
   npm start
   ```

4. Access the application at: http://localhost:3000

## 👨‍💻 Development

### 📜 Available Scripts

- 🚀 `npm start` - Start development server
- 🧪 `npm test` - Run tests
- 📦 `npm run build` - Build for production
- 📝 `npm run lint` - Run ESLint
- 💅 `npm run format` - Format code with Prettier
- ✅ `npm run type-check` - Run TypeScript type checking

### 🧩 Component Development

1. Create new components in the appropriate directory under `src/components/`
2. Follow the component structure:
   ```typescript
   import React from 'react';
   import { styled } from '@mui/material/styles';
   
   interface ComponentProps {
     // Props interface
   }
   
   const StyledComponent = styled('div')({
     // Styles
   });
   
   export const Component: React.FC<ComponentProps> = ({ /* props */ }) => {
     return (
       <StyledComponent>
         {/* Component content */}
       </StyledComponent>
     );
   };
   ```

### 📦 State Management

The application uses Redux Toolkit for state management. To add new state:

1. Create a new slice in `src/store/slices/`
2. Add the slice to the store configuration
3. Use the generated hooks in components

Example slice:
```typescript
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface State {
  // State interface
}

const initialState: State = {
  // Initial state
};

const slice = createSlice({
  name: 'feature',
  initialState,
  reducers: {
    // Reducers
  },
});

export const { actions } = slice;
export default slice.reducer;
```

### 🧪 Testing

1. Write tests in the `__tests__` directory next to the component
2. Use React Testing Library for component testing
3. Run tests with `npm test`

Example test:
```typescript
import { render, screen } from '@testing-library/react';
import { Component } from './Component';

describe('Component', () => {
  it('renders correctly', () => {
    render(<Component />);
    expect(screen.getByText('Expected Text')).toBeInTheDocument();
  });
});
```

## 🎨 Styling

The application uses Material-UI's styling solution. To add custom styles:

1. Use the `styled` API for component-specific styles
2. Use the theme for global styles
3. Follow the Material-UI design system

Example styled component:
```typescript
import { styled } from '@mui/material/styles';

export const StyledComponent = styled('div')(({ theme }) => ({
  padding: theme.spacing(2),
  backgroundColor: theme.palette.background.paper,
}));
```

## 🌐 API Integration

API calls are handled through services in the `src/services` directory. Each service:

1. Uses Axios for HTTP requests
2. Handles authentication
3. Includes error handling
4. Returns typed responses

Example service:
```typescript
import axios from 'axios';
import { API_URL } from '../config';

export const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const getData = async () => {
  const response = await api.get('/endpoint');
  return response.data;
};
```

## 🚀 Deployment

1. Build the application:
   ```bash
   npm run build
   ```

2. Deploy the contents of the `build` directory to your hosting service

3. Configure environment variables in your hosting environment

## 🤝 Contributing

1. 📝 Follow the component structure and coding standards
2. 🧪 Write tests for new features
3. 📚 Update documentation as needed
4. 📬 Submit pull requests with clear descriptions

## 📄 License

MIT License - see the [LICENSE](../LICENSE) file for details
