import React from 'react';
import {
  Grid,
  Paper,
  Typography,
  Box,
  Card,
  CardContent,
  List,
  ListItem,
  ListItemText,
  ListItemIcon,
  Divider,
} from '@mui/material';
import {
  AccountBalance as AccountIcon,
  TrendingUp as TrendingUpIcon,
  Notifications as NotificationIcon,
} from '@mui/icons-material';

export const Dashboard: React.FC = () => {
  const recentTransactions = [
    { id: 1, description: 'Grocery Shopping', amount: -85.50, date: '2024-03-15' },
    { id: 2, description: 'Salary Deposit', amount: 3000.00, date: '2024-03-14' },
    { id: 3, description: 'Electric Bill', amount: -120.75, date: '2024-03-13' },
  ];

  const accounts = [
    { id: 1, name: 'Checking Account', balance: 5240.50, type: 'Checking' },
    { id: 2, name: 'Savings Account', balance: 15000.00, type: 'Savings' },
    { id: 3, name: 'Investment Account', balance: 25000.00, type: 'Investment' },
  ];

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Dashboard
      </Typography>
      <Grid container spacing={3}>
        {/* Account Summary */}
        <Grid item xs={12} md={8}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Account Summary
            </Typography>
            <Grid container spacing={2}>
              {accounts.map((account) => (
                <Grid item xs={12} sm={4} key={account.id}>
                  <Card>
                    <CardContent>
                      <Typography color="textSecondary" gutterBottom>
                        {account.type}
                      </Typography>
                      <Typography variant="h6">
                        {account.name}
                      </Typography>
                      <Typography variant="h5" color="primary">
                        ${account.balance.toLocaleString()}
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
              ))}
            </Grid>
          </Paper>
        </Grid>

        {/* Recent Transactions */}
        <Grid item xs={12} md={4}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Recent Transactions
            </Typography>
            <List>
              {recentTransactions.map((transaction, index) => (
                <React.Fragment key={transaction.id}>
                  <ListItem>
                    <ListItemIcon>
                      <TrendingUpIcon color={transaction.amount > 0 ? 'success' : 'error'} />
                    </ListItemIcon>
                    <ListItemText
                      primary={transaction.description}
                      secondary={transaction.date}
                    />
                    <Typography
                      color={transaction.amount > 0 ? 'success.main' : 'error.main'}
                      variant="body1"
                    >
                      ${Math.abs(transaction.amount).toFixed(2)}
                    </Typography>
                  </ListItem>
                  {index < recentTransactions.length - 1 && <Divider />}
                </React.Fragment>
              ))}
            </List>
          </Paper>
        </Grid>

        {/* Quick Actions */}
        <Grid item xs={12}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Quick Actions
            </Typography>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={4}>
                <Card>
                  <CardContent>
                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                      <AccountIcon color="primary" sx={{ mr: 1 }} />
                      <Typography variant="h6">New Account</Typography>
                    </Box>
                    <Typography variant="body2" color="textSecondary">
                      Open a new checking or savings account
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
              <Grid item xs={12} sm={4}>
                <Card>
                  <CardContent>
                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                      <TrendingUpIcon color="primary" sx={{ mr: 1 }} />
                      <Typography variant="h6">Transfer</Typography>
                    </Box>
                    <Typography variant="body2" color="textSecondary">
                      Transfer money between accounts
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
              <Grid item xs={12} sm={4}>
                <Card>
                  <CardContent>
                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                      <NotificationIcon color="primary" sx={{ mr: 1 }} />
                      <Typography variant="h6">Alerts</Typography>
                    </Box>
                    <Typography variant="body2" color="textSecondary">
                      Set up account notifications
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            </Grid>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
}; 