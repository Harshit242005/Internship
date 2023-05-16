

import React, { useState } from 'react';
import { Link } from 'react-router-dom'
import {
    Checkbox,
    FormControlLabel,
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Typography,
} from '@mui/material';
import { ExpandMore as ExpandMoreIcon } from '@mui/icons-material';

interface Department {
    department: string;
    sub_departments: string[];
}

const departmentsData: Department[] = [
    {
        department: 'customer_service',
        sub_departments: ['support', 'customer_success'],
    },
    {
        department: 'design',
        sub_departments: ['graphic_design', 'product_design', 'web_design'],
    },
];

const ChoiceData = () => {
    const [selectedDepartments, setSelectedDepartments] = useState<string[]>([]);
    const [expandedDepartments, setExpandedDepartments] = useState<string[]>([]);

    const handleExpandClick = (department: Department) => {
        if (expandedDepartments.includes(department.department)) {
            setExpandedDepartments((prevExpanded) =>
                prevExpanded.filter((dep) => dep !== department.department)
            );
        } else {
            setExpandedDepartments((prevExpanded) => [...prevExpanded, department.department]);
        }
    };

    const handleDepartmentChange = (department: Department) => {
        if (selectedDepartments.includes(department.department)) {
            setSelectedDepartments((prevDepartments) =>
                prevDepartments.filter(
                    (dep) => dep !== department.department && !department.sub_departments.includes(dep)
                )
            );
        } else {
            setSelectedDepartments((prevDepartments) => [
                ...prevDepartments,
                department.department,
                ...department.sub_departments,
            ]);
        }
    };

    const handleSubDepartmentChange = (department: Department, subDepartment: string) => {
        if (selectedDepartments.includes(subDepartment)) {
            setSelectedDepartments((prevDepartments) =>
                prevDepartments.filter((dep) => dep !== subDepartment && dep !== department.department)
            );
        } else {
            const allSubDepartmentsSelected = department.sub_departments.every((subDep) =>
                selectedDepartments.includes(subDep)
            );
            if (allSubDepartmentsSelected) {
                setSelectedDepartments((prevDepartments) =>
                    prevDepartments.filter((dep) => dep !== department.department)
                );
            }
            setSelectedDepartments((prevDepartments) => [...prevDepartments, subDepartment]);
        }
    };

    const isDepartmentExpanded = (department: Department) => {
        return expandedDepartments.includes(department.department);
    };

    const isDepartmentSelected = (department: Department) => {
        const allSubDepartmentsSelected = department.sub_departments.every((subDep) =>
            selectedDepartments.includes(subDep)
        );
        return selectedDepartments.includes(department.department) || allSubDepartmentsSelected;
    };

    return (
        <div>
            <Link to="/"><button style={{ backgroundColor: 'black', color: 'white', padding: '10px 20px', width: '200px', height: '50px' }}>Go back</button></Link>

            <Typography variant="h6" gutterBottom>
                Departments
            </Typography>

            {departmentsData.map((department) => (
                <Accordion
                    key={department.department}
                    expanded={isDepartmentExpanded(department)}
                    onChange={() => handleExpandClick(department)}
                >
                    <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                        <FormControlLabel
                            control={
                                <Checkbox
                                    checked={isDepartmentSelected(department)}
                                    indeterminate={
                                        selectedDepartments.some((dep) => department.sub_departments.includes(dep)) &&
                                        !selectedDepartments.includes(department.department)
                                    }
                                    onChange={() => handleDepartmentChange(department)}
                                />
                            }
                            label={department.department}
                        />
                    </AccordionSummary>
                    <AccordionDetails>
                        <div>
                            {department.sub_departments.map((subDepartment) => (
                                <FormControlLabel
                                    key={subDepartment}
                                    control={
                                        <Checkbox
                                            checked={selectedDepartments.includes(subDepartment)}
                                            onChange={() => handleSubDepartmentChange(department, subDepartment)}
                                        />
                                    }
                                    label={subDepartment}
                                />
                            ))}
                        </div>
                    </AccordionDetails>
                </Accordion>
            ))}

        </div>
    );
};

export default ChoiceData;